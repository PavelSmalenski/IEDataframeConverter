# Dataframe converter

This program was created to automate the process of converting IE dataframe structures into COBOL data definitions

## Features

- Parsing LISTFD utility's output that produses IE Dataframe structures
- Printing resulting COBOL code into files on PC
- Printing resulting COBOL code into MVS PDS members via FTP

## Installation

1. Build an app
2. For using FTP ability:
    1. Project contains Configs/mvsFtpSettings.json template which will be copied to output directory during Build
    2. Fill Configs/mvsFtpSettings.json with correct parameters of desired MVS system

## Usage

1. Unload SYSPRINT of a step that's been calling LISTFD utility
2. Put it into dataframesSysprint.txt near app's .exe file
3. Run the app
4. Choose output option
5. Done - check the resulting COBOL

## TODO's

- Allow custom input file location
    - ...or simply take SYSPRINT directly from MVS (?)
- Add output formatting options for better flexibility
- UI?

## Version
v0.1 - Initial