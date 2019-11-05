import pandas as pd
from extract_words import get_freq, normalize_words
from gutenberg.query import get_metadata
from collections import Counter

interested_genres = set(normalize_words('mystery romance scifi fantasy', stem=False))
num_books = 1000
size = '1000-books'
filter_genres = False

data = pd.read_csv('gutenberg.csv', index_col='gutenberg id')
ids = data.index

def get_genre(i):
    genres = [word for subject in get_metadata('subject', i) for word in normalize_words(subject, stem=False) ]
    filtered = [genre for genre in genres if genre in interested_genres]
#     print(get_metadata('subject', i) )
    if len(filtered) > 0:
        genres = filtered
        
    if len(genres) == 0:
        return ""
    
    return Counter(genres).most_common(1)[0][0]
    
    
genres = pd.DataFrame([get_genre(i) for i in ids], ids, ['genre'])
with_genre = pd.concat([data, genres], axis=1)

if filter_genres:
    filtered_by_genre = with_genre[with_genre.genre.isin(interested_genres)]
else:
    filtered_by_genre = with_genre
	
sample = filtered_by_genre.sample(num_books)
ids = sample.index

totals_and_freqs = pd.DataFrame([[*get_freq(i)] for i in ids], ids, ['total word count', 'word frequencies'])

result = pd.concat([sample, totals_and_freqs], axis=1)

result.to_csv(f'{size}-dataset.csv')