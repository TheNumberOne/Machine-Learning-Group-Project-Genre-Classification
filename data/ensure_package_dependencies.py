#!/bin/python

import nltk
nltk.download('stopwords')
nltk.download('punkt')

from gutenberg.acquire import get_metadata_cache
cache = get_metadata_cache()
if not cache.exists:
    cache.populate()