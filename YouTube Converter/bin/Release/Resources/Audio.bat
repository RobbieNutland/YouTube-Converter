@echo off

#Check for updates
youtube-dl -U



IF NOT EXIST "%USERPROFILE%\YouTube Conversions" (

mkdir "%USERPROFILE%\YouTube Conversions"

)


IF "%CROPPED%"=="" (

youtube-dl -f bestaudio --extract-audio --audio-format mp3 -o "%OUTPATH%\%NAME%.%%(ext)s" %URL% > ".\Resources\YTC-Log.txt" 2>&1

) ELSE (

youtube-dl -f bestaudio --extract-audio --postprocessor-args "-ss %STARTTIME% -to %FINISHTIME%" --audio-format mp3 -o "%OUTPATH%\%NAME%.%%(ext)s" %URL% > ".\Resources\YTC-Log.txt" 2>&1

)
exit