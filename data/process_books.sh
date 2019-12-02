#!/bin/sh

rm -rf raw_gutenberg_processed
cp -r raw_gutenberg raw_gutenberg_processed
cd raw_gutenberg_processed
rm harvest*
rm robot*

unzip -oj \*.zip \*.txt > /dev/null

rm *.zip
rm *-8.txt
rm readme.txt
rm *-0.txt

echo "gutenberg id,title" > ../gutenberg.csv
grep "Title:" * | sed -E 's/\r//;s/Title: //;s/.txt:/,/;s/"/""/g;s/^([0-9]+),(.*)$/\1,"\2"/' >> ../gutenberg.csv
