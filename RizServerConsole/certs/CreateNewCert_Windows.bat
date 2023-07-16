@echo off
echo Create a new cert need install OpenSSL-Win64. You can download it in "https://slproweb.com/products/Win32OpenSSL.html"
openssl req -newkey rsa:2048 -nodes -keyout 0.key -x509 -days 365 -out 0.cer
openssl pkcs12 -export -in 0.cer -inkey 0.key -out cert_output.pfx
pause