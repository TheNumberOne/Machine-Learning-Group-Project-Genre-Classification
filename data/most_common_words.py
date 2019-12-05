#!/bin/python

import pandas as pd
from extract_words import extract_words
from collections import Counter


data = pd.read_csv('results/gutenberg.csv', index_col='gutenberg id')

words = Counter()
for gutenberg_id in data.index:
    words.update(extract_words(gutenberg_id))

most_common_pd = pd.DataFrame(data=words.most_common(), columns=["words", "freq"])
most_common_pd.to_csv("results/most-common-words.csv")
