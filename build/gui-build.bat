echo off
set op=%1
if not exist %op%\res mkdir %op%\res
copy /v /y ..\..\res\* %op%\res
