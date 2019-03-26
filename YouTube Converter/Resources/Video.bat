@echo off

#Check for updates
youtube-dl -U



IF NOT EXIST "%USERPROFILE%\YouTube Conversions" (

mkdir "%USERPROFILE%\YouTube Conversions"

)


IF "%CROPPED%"=="" (

youtube-dl -f mp4 -o "%OUTPATH%\%NAME%.mp4" %URL% > "Resources\YTC-Log.txt" 2>&1

) ELSE (

youtube-dl --postprocessor-args "-ss %STARTTIME% -to %FINISHTIME%" -f mp4 -o "%OUTPATH%\%NAME%.mp4" %URL% > "Resources\YTC-Log.txt" 2>&1

)
exit