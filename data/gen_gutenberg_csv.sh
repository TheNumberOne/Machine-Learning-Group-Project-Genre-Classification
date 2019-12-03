
echo "gutenberg id,title" > results/gutenberg.csv
grep "Title:" raw_gutenberg_processed/* | sed -E 's/raw_gutenberg_processed\///; /Binary file/d; s/\r//; s/Title: //; s/.txt:/,/; s/"/""/g; s/^([0-9]+),(.*)$/\1,"\2"/' >> results/gutenberg.csv
