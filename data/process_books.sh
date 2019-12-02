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
rm 89-*.txt
rm 10681-*.txt

../gen_gutenberg_csv.sh