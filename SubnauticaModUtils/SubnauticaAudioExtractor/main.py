import re


def create_sound_tuple(sound_path_string):
    new_name = sound_path_string.strip().title().replace("_", "").\
        replace("Event:", "").replace("/", "").replace(":", "").replace(" ", "").\
        replace("Bz", "")
    # Now split the line by capitalisation, so we can split out into classes
    words = re.findall('[A-Z][^A-Z]*', new_name)
    # We'll use the first part as the 'Class', rest as the attribute
    sound_class = words[0]
    words.remove(sound_class)
    sound_field = ''.join(words)
    sound_path = sound_path_string.strip()
    sound_tuple = (sound_class, sound_field, sound_path)
    return sound_tuple


def create_sound_tuples_from_file(in_file_name):
    print(f"Processing: {in_file_name}")
    # To remove duplicates
    lines_seen = set()
    sound_tuples = set()
    # Open file
    file_in = open(in_file_name, 'r')
    in_file_lines = file_in.readlines()
    # Parse content
    for in_file_line in in_file_lines:
        if in_file_line not in lines_seen:
            if len(in_file_line) > 1:
                sound_tuple = create_sound_tuple(sound_path_string=in_file_line)
                sound_tuples.add(sound_tuple)
                lines_seen.add(in_file_line)
    # Close files
    file_in.close()
    return sound_tuples


def write_output(sound_tuples, out_file_name, parent_class):
    print(f"Writing: {out_file_name}")
    file_out = open(out_file_name, 'w')
    current_class = ""
    for sound_tuple in sound_tuples:
        line_class = sound_tuple[0]
        if line_class != current_class:
            if current_class != "":
                file_out.write(f"}}\n\n")
            current_class = line_class
            file_out.write(f"public class {line_class}\n{{\n")
        file_out.write(f"\tpublic static {parent_class} {sound_tuple[1]} = new {parent_class}(\"{sound_tuple[2]}\");\n")
    file_out.write(f"}}")
    file_out.close()


def gen_audio_code(in_file_name, out_file_name, parent_class):
    sound_tuples = create_sound_tuples_from_file(in_file_name=in_file_name)
    sorted_sound_tuples = sorted(sound_tuples, key=lambda tup: (tup[0], tup[1]))
    write_output(sound_tuples=sorted_sound_tuples, out_file_name=out_file_name, parent_class=parent_class)


# Process the sound path file
if __name__ == '__main__':
    gen_audio_code(in_file_name='bz_sounds.txt', out_file_name='bz_sounds_code.txt', parent_class="BZGameSound")
    gen_audio_code(in_file_name='sn_sounds.txt', out_file_name='sn_sounds_code.txt', parent_class="SNGameSound")
