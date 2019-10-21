#!/bin/sh

mkdir -p raw_gutenburg
wget -P raw_gutenburg -m -H -nd "http://www.gutenberg.org/robot/harvest?filetypes[]=txt&langs[]=en" 
