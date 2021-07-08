def create_code_line(name):
    new_name = name.strip().title().replace("_", "").\
        replace("Event:", "").replace("/", "").replace(":", "").replace(" ", "").\
        replace("Bz", "")
    code_line = f"public const string {new_name} = \"{name.strip()}\";\n"
    return code_line


def create_audio_code(in_file_name, out_file_name):
    print(f"Processing: {in_file_name} to {out_file_name}")
    # To remove duplicates
    lines_seen = set()
    # Open files
    file_in = open(in_file_name, 'r')
    file_out = open(out_file_name, 'w')
    in_file_lines = file_in.readlines()

    # Parse content
    for in_file_line in in_file_lines:
        if in_file_line not in lines_seen:
            if len(in_file_line) > 1:
                new_line = create_code_line(in_file_line)
                file_out.write(new_line)
                lines_seen.add(in_file_line)
    # Close files
    file_in.close()
    file_out.close()


# Process the sound path file
if __name__ == '__main__':
    create_audio_code('sounds.txt', 'sounds_code.txt')
