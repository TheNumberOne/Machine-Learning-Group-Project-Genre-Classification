#!/bin/sh

rm -rf raw_gutenburg_processed
cp -r raw_gutenburg raw_gutenburg_processed
cd raw_gutenburg_processed
rm harvest*
rm robot*

unzip -oj \*.zip \*.txt > /dev/null

rm *.zip
rm *-8.txt
rm readme.txt
rm *-0.txt

echo "gutenberg id,title" > ../gutenburg.csv
grep "Title:" * | sed -E 's/\r//;s/Title: //;s/.txt:/,/;s/"/""/;s/^([0-9]+),(.*)$/\1,"\2"/' >> ../gutenburg.csv
