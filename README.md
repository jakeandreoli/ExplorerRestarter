# ExplorerRestarter

A simple C# application that monitors for a specific key combination (`Right Ctrl + Question Mark + Enter`) to restart the Windows Explorer process.

## Motivation

This was made entirely because Windows 11 is an unfinished product. I've grown tired of windows going underneath my taskbar and not being able to get it back without restarting the explorer process. This solution is a lot faster and more convenient than the traditional method of opening the task manager and restarting the process from there.

There are other solutions out there that do the same thing, but I wanted to make my own version of it primarily to get back into using C#.

## Installation

### **Prerequisites:**
1. **Visual Studio** 2019 or later with:
    - **.NET Desktop Development** workload
2. **.NET Framework 4.8**

```bash
git clone https://github.com/jakeandreoli/ExplorerRestarter.git
cd ExplorerRestarter
```

### Build

1. Open the `ExplorerRestarter.sln` file in Visual Studio.
2. Press `Ctrl + Shift + B` to build the project.
3. That's it!

## Add to Startup

1. Press `Win + R` to open the Run dialog.
2. Type `shell:startup` and press Enter.
3. Copy the `ExplorerRestarter.exe` file into the folder that opens.

## Contributing

If you would like to contribute to this project, please feel free to submit a pull request. I will review it as soon as possible.

## Notes

This README was generated using GitHub Copilot. As such, some details may be missing. If you have additional details that you believe would help others, please feel free to submit a pull request.

## License

This project is licensed under the MIT License.

