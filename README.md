# NotPowerShell
This is a fork from Ben0xA's original Not PowerShell (nps) project: https://github.com/Ben0xA/nps.

Thus far, the only changes have been to do some minor refactoring to better reflect .NET project naming standards. The rest of this readme document is original and still valid in reference to the executable.

One matter of note: the downloaded executable has been built from the updated project and updated accordingly. It should still behave as expected.

Usage
```c:\Downloads>nps.exe
 usage:
 nps.exe "{powershell single command}"
 nps.exe "& {commands; semi-colon; separated}"
 nps.exe -encodedcommand {base64_encoded_command}
 nps.exe -encode "commands to encode to base64"
 nps.exe -decode {base64_encoded_command}
```

Single Commands
```
 c:\Downloads>nps.exe Get-Date
 12/18/2015 2:19:37 PM
```

Multiple Commands 
```
 c:\Downloads>nps.exe "& Get-Date; Write-Output 'Ohai there'"
 12/18/2015 2:19:49 PM
 Ohai there
```

Encoding
```
 c:\Downloads>nps.exe -encode "& Get-Date; Write-Output 'Ohai there'"
 JgAgAEcAZQB0AC0ARABhAHQAZQA7ACAAVwByAGkAdABlAC0ATwB1AHQAcAB1AHQAIAAnAE8AaABhAGkAIAB0AGgAZQByAGUAJwA=
```

Decoding
```
 c:\Downloads>nps.exe -decode JgAgAEcAZQB0AC0ARABhAHQAZQA7ACAAVwByAGkAdABlAC0ATwB1AHQAcAB1AHQAIAAnAE8
 AaABhAGkAIAB0AGgAZQByAGUAJwA=
 & Get-Date; Write-Output 'Ohai there'
```

Running Encoded Command
```
 c:\Downloads>nps.exe -encodedcommand JgAgAEcAZQB0AC0ARABhAHQAZQA7ACAAVwByAGkAdABlAC0ATwB1AHQAcAB1AHQ
 AIAAnAE8AaABhAGkAIAB0AGgAZQByAGUAJwA=
 12/18/2015 2:20:19 PM
 Ohai there
```

Same Encoded Command works in PowerShell
```
 c:\Downloads>powershell.exe -noprofile -encodedcommand JgAgAEcAZQB0AC0ARABhAHQAZQA7ACAAVwByAGkAdABlA
 C0ATwB1AHQAcAB1AHQAIAAnAE8AaABhAGkAIAB0AGgAZQByAGUAJwA=

 Friday, December 18, 2015 2:20:30 PM
 Ohai there
```
