#!/bin/sh

mkdir -p raw_gutenberg
wget -P raw_gutenberg -m -H -nd "http://www.gutenberg.org/robot/harvest?filetypes[]=txt&langs[]=en" 
