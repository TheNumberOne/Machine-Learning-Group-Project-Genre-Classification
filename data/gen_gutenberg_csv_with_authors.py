#!/bin/python

import pandas as pd
from gutenberg.query import get_metadata

data = pd.read_csv("results/gutenberg.csv", index_col="gutenberg id")
authors = data.index.to_series() \
	.map(lambda gutenberg_id: get_metadata('author', gutenberg_id)) \
	.to_frame('authors')

data_with_authors = pd.concat([data, authors], axis=1)
data_with_authors.to_csv("results/gutenberg-with-authors.csv")