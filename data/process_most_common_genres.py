#!/bin/python

import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from collections import Counter
import sys

if len(sys.argv) > 1:
	size = sys.argv[1]
else:
	size = -1
num_most_common = 5

if size != -1:
	books = pd.read_csv(f"results/{size}-dataset.csv")
else:
	books = pd.read_csv(f"results/dataset.csv")
	
genre_counts = Counter(books.genre)
genre_freqs = pd.DataFrame(data=genre_counts.most_common(num_most_common), columns=["genre", "freq"])
genre_freqs.to_csv(f"results/{num_most_common}-most-common-genres.csv", index=False)

most_common_genres = { genre: freq for genre, freq in genre_counts.most_common() if freq >= 10 }
books_with_common_genres = books[books.genre.isin(most_common_genres)]

books_with_common_genres.to_csv(f"results/{len(books_with_common_genres)}-dataset-with-common-genres.csv", index=False)