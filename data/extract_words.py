#!/usr/bin/python

from gutenberg.cleanup import strip_headers
import nltk
from nltk.corpus import stopwords
from nltk.stem.porter import PorterStemmer
import time
from collections import Counter 
import string
import chardet

ps = PorterStemmer()
stop_words = set(stopwords.words('english'))
punc_remover = str.maketrans('', '', string.punctuation)

def get_freq(gutenberg_id):
# 	start = time.process_time()

	try:
		with open(f"raw_gutenberg_processed/{gutenberg_id}.txt", 'rb') as f:
			data = f.read()
			encoding = chardet.detect(data)['encoding']
			data = data.decode(encoding)
	except:
		raise Exception(f"book #{gutenberg_id}")
		
	cleaned = strip_headers(data).strip()
	words = normalize_words(cleaned)
	total_words = len(words)
	freq = dict(Counter(words))
# 	print(time.process_time() - start)	
	
	return total_words, freq

def normalize_words(text, stem=True):
	tokenized = tokenized = nltk.word_tokenize(text)
	words = [word.lower().translate(punc_remover) for word in tokenized]
	filtered_words = [word for word in words if word not in stop_words and len(word) > 0]
	if stem:
		stemmed = [ps.stem(word) for word in filtered_words]
	else:
		stemmed = filtered_words
	
	return stemmed
	

if __name__ == "__main__":
	import sys
	
	book_id = sys.argv[1]
	w, f = get_freq(book_id)
	
	print(w)
	print(f)
