#!/usr/bin/python

from gutenberg.cleanup import strip_headers
import nltk
from nltk.corpus import stopwords
from nltk.stem.porter import PorterStemmer
import time

ps = PorterStemmer()
stop_words = set(stopwords.words('english'))

def get_freq(gutenberg_id):
	start = time.process_time()

	with open(f"raw_gutenburg_processed/{book_id}.txt", "r") as f:
		data = f.read()
		
	cleaned = strip_headers(data).strip()
	tokenized = tokenized = nltk.word_tokenize(cleaned)
	words = [word.lower() for word in tokenized if word.isalpha()]


	filtered_words = [word for word in words if word not in stop_words]
	stemmed = [ps.stem(word) for word in filtered_words]
	total_words = len(stemmed)
	freq = {i:stemmed.count(i) for i in set(stemmed)}
	print(time.process_time() - start)	
	
	return total_words, freq

if __name__ == "__main__":
	import sys
	
	book_id = sys.argv[1]
	w, f = get_freq(book_id)
	
	print(w)
	print(f)
