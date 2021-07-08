# Subnautica Audio Extractor



These scripts allow you to extract all FMOD audio paths from Subnautica: Below Zero.

## Instructions for use

1. Install [Unity Explorer](https://github.com/sinai-dev/UnityExplorer/releases).

2. Run the C# code from `extract_audio.css.`

3. Open the Log file and copy the extracted contents to a text file called `sounds.txt`. Only copy text from after `UnityExplorer 4.1.7 initialized` and before `Invoked REPL (no return value)` at the end.

4. Save this file in the same folder as main.py`.

5. Run `main.py`:

   ```
   python main.py
   ```

6. Open `sounds_code.txt` and paste into Visual Studio.
