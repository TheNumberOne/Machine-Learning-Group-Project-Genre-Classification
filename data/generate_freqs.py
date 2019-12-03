#!/bin/python

import pandas as pd
from extract_words import get_freq, normalize_words
from gutenberg.query import get_metadata
from collections import Counter
import sys

interested_genres = set(normalize_words('mystery romance scifi fantasy', stem=False))
if len(sys.argv) > 1:
	num_books = int(sys.argv[1])
else:
	num_books = -1
filter_genres = False

data = pd.read_csv('results/gutenberg-with-authors-genres.csv', index_col='gutenberg id')

if num_books != -1:
	sample = data.sample(num_books)
else:
	sample = data
	
ids = sample.index

totals_and_freqs = pd.DataFrame([[*get_freq(i)] for i in ids], ids, ['total word count', 'word frequencies'])

result = pd.concat([sample, totals_and_freqs], axis=1)
result = result[result['word frequencies'] != dict()]
if num_books == -1:
	result.to_csv("results/dataset.csv")
else:
	result.to_csv(f'results/{num_books}-dataset.csv')