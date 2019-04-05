@echo off
set outpath=B:\plugins\ScribanEngine
del /s /q %outpath%\*
dotnet publish -o %outpath%
echo %date% %time%