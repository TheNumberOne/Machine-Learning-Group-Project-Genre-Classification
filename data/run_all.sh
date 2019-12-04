set -e

mkdir -p results

if [ ! -d raw_gutenberg ]; then
    echo 'Downloading Project Gutenberg'
	./download_books.sh
    echo 'Done downloading Project Gutenberg'
fi

if [ ! -d raw_gutenberg_processed ]; then
    echo 'Processing Project Gutenberg'
	./process_books.sh
    echo 'Done processing Project Gutenberg'
fi

if [ ! -d raw_open_library ]; then
    echo 'Downloading Open Library data'
	./download_metadata.sh
    echo 'Done Downloading'
fi

if [ ! -f results/gutenberg.csv ]; then
    echo 'Creating gutenberg.csv'
	./gen_gutenberg_csv.sh
    echo 'Done creating gutenberg.csv'
fi

echo "Ensuring python data dependencies are downloaded"
./ensure_package_dependencies.py
echo "Done ensuring python data dependencies are downloaded"

# if [ ! -f results/gutenberg-with-authors.csv ]; then
#     echo 'Adding authors to csv'
# 	./gen_gutenberg_csv_with_authors.py
#     echo 'Done adding authors'
# fi

if [ ! -f raw_open_library/openlibrary.sqlite3 ]; then
    echo 'Creating database for open library'
	./create_open_library_database.py
    echo 'Done creating database for open library'
fi

if [ ! -f results/gutenberg-with-authors-genres.csv ]; then
    echo 'Adding genres to csv'
	./add_genres.py
    echo 'Done adding genres'
fi

for size in 10 100 1000
do
	if [ ! -f "results/$size-dataset.csv" ]; then
        echo "Creating results/$size-dataset.csv"
		./generate_freqs.py $size
        echo "Done creating results/$size-dataset.csv"
	fi
    echo "Processing most common genres for results/$size-dataset.csv"
	./process_most_common_genres.py $size
    echo "Done processing most common genres for results/$size-dataset.csv"
done

if [ ! -f "results/dataset.csv" ]; then
	./generate_freqs.py
fi

echo 'Finding most common genres'
./process_most_common_genres.py
echo 'Done finding most common genres'

if [ ! -f "results/most-common-words.csv" ]; then
    echo 'Finding most common words'
    ./most_common_words.py
    echo 'Done finding most common words'
fi

echo 'Compressing berg files'
find results -type f -size +100M -exec zip -u {}.zip {} \;
echo "Done compressing"
echo 'WARNING: MAKE SURE TO NOT COMMIT BIG FILES TO GIT. BIG FILES:'
find results -type f -size +100M