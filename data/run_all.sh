set -e

mkdir -p results

if [ ! -d raw_gutenberg ]; then
	./download_books.sh
fi

if [ ! -d raw_gutenberg_processed ]; then
	./process_books.sh
fi

if [ ! -d raw_open_library ]; then
	./download_metadata.sh
fi

if [ ! -f results/gutenberg.csv ]; then
	./gen_gutenberg_csv.sh
fi

if [ ! -f results/gutenberg-with-authors.csv ]; then
	./gen_gutenberg_csv_with_authors.py
fi

if [ ! -f raw_open_library/openlibrary.sqlite3 ]; then
	./create_open_library_database.py
fi

if [ ! -f results/gutenberg-with-authors-genres.csv ]; then
	./add_genres.py
fi

for size in 10 100 1000
do
	if [ ! -f "results/$size-dataset.csv" ]; then
		./generate_freqs.py $size
	fi
	./process_most_common_genres.py $size
done

if [ ! -f "results/dataset.csv" ]; then
	./generate_freqs.py
fi

./process_most_common_genres.py