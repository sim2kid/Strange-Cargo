@echo off

call :treeProcess
goto :eof

:treeProcess

for %%F in (*.wav) DO (
    ffmpeg -i "%%F" "%%~nF.mp3"
    DEL %%F
)

for /D %%d in (*) do (
    cd %%d
    call :treeProcess
    cd ..
)

exit /b

timeout 60