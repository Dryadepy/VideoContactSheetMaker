# VideoContactSheetMaker
Video Contact Sheet Maker is a cross-platform software to create contact sheets from video files. It is meant to be a tool creating contact sheets quickly and easy and without installing any software.

# Features
- Cross-platform
- Various default profiles (easy to use)
- Customizable
- Batch mode
- Portable

# Requirements
FFmpeg and FFprobe is required.

#### Windows user:
Get latest package from https://ffmpeg.zeranoe.com/builds/ and extract ffmpeg.exe and ffprobe into the same directory where VideoContactSheetMaker.exe is placed in.

#### Linux user:
Install latest ffmpeg and ffprobe the usual way and verify PATH environment variable is set.

# How to use:

```
Available commands:

  -i Path        Required: Include a file or folder
  -s Path        Optional: Path to profile or a default profile name, otherwise it uses standard profile
  -r             Optional: Recursive, applies to included folders
  -o Path        Optional: Output folder, by default its the same folder as the file

Examples:
VideoContactSheetMaker -i "C:\Videos\
VideoContactSheetMaker -i "C:\Videos\ -s default2
VideoContactSheetMaker -i "C:\Videos\ -i "C:\Videos2\" -r -s "C:\profile.xml" -o "C:\Screens\"
```

# Screenshots
Default profile 1
![sample](https://user-images.githubusercontent.com/46010672/51354411-6d7ab980-1aab-11e9-82f2-fcdf9ae9f8a7.jpg)
Default profile 3
![sample](https://user-images.githubusercontent.com/46010672/51354428-779cb800-1aab-11e9-90ff-b39ad823b4a4.jpg)


# License
Video Contact Sheet Maker is licensed under GPLv3  
Video Contact Sheet Maker uses ffmpeg / ffprobe (not included) which is licesed under LGPL 2.1 / GPL v2
