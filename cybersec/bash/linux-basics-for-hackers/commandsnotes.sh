#bin/bash

#echo "This is the end"
#apt-key
#grep
#sort
#rm

nl <filename> #number lines in file as aoutput in console.
head -5 filename #primeras 5 lineas dela rchivo
tail -2 filename #ultimas 2 lineas del archivo

nl ssh_config | grep user > /home/kali/Desktop/mysshtest.txt #encuentra el string user en el archivo ssh_config numera lineas y guardalo en un txt en el escritorio
tail -507 filename | head -6  # las siguientes 6 lineas del archivo de las ultimas 507 lineas 
nl 
#sed : permite encotrar ocurrencias de una palabra y ejecutar una operacion en ella.

sed s/wordToReplace/replacerWord/g fileToFindOcurrence > fileToSaveNewChange # g is globally


grep




awk

