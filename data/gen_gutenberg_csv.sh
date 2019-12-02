
echo "gutenberg id,title" > ../gutenberg.csv
grep "Title:" * | sed -E 's/\r//; s/Title: //; s/.txt:/,/; s/"/""/g; s/^([0-9]+),(.*)$/\1,"\2"/' >> ../gutenberg.csv
