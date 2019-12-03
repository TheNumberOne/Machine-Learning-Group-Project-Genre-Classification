#!/bin/python

import sqlite3
from sqlite3 import Error
import csv
import json
import itertools
import sys

def create_connection():
    """ create a database connection to a SQLite database """
    conn = sqlite3.connect("raw_open_library/openlibrary.sqlite3")
    return conn

def create_tables(conn):
    curs = conn.cursor()
    curs.execute('''
        create table if not exists authors (
            author_id text primary key,
            name text
        )
    ''')
    curs.execute('''create table if not exists books (
        book_id text primary key, 
        title text, 
        author_ids text,
        genres text
    )''')
    
    
def get_sql_book_data(lines):
    for ol_type, ol_id, revision, last_modified, record_json in lines:
        # only process editions
        if ol_type != "/type/edition":
            continue

        record = json.loads(record_json)

        # useless record
        if "title" not in record or "authors" not in record or "genres" not in record:
            continue

        # extract data
        title = record["title"]

        # convert [{"key": "id1"}, {"key": "id2"}] to ["id1", "id2"]
        authors = [d["key"] for d in record["authors"]]
        authors_json = json.dumps(authors)

        genres = record["genres"]
        genres_json = json.dumps(genres)
        yield ol_id, title, authors_json, genres_json
        
        
def get_sql_author_data(lines):
    for ol_type, ol_id, revision, last_modified, record_json in lines:
        # ignore non-authors
        if ol_type != "/type/author":
            continue
            
        record = json.loads(record_json)
        
        if "name" not in record:
            continue
        title = record["name"]
        yield ol_id, title
        
def add_authors_to_database(conn, data):
    conn.executemany('''
        insert into authors(author_id, name)
        values (?, ?)
    ''', data)
    
def add_books_to_database(conn, data):
    conn.executemany('''
        insert into books(book_id, title, author_ids, genres)
        values (?, ?, ?, ?)
    ''', data)

def create_indices(conn):
    conn.execute('''
        create index if not exists books_title_index
        on books (title collate nocase);
    ''')
    conn.execute('''
        create index if not exists authors_name_index
        on authors (name collate nocase);
    ''')
	
def create_db():
	# some fields are absolute units
	csv.field_size_limit(sys.maxsize)
	processed_authors = False
	processed_books = False

	with create_connection() as conn:
		create_tables(conn)
		if not processed_authors:
			with open("raw_open_library/all_metadata.txt") as file:
				lines = csv.reader(file, dialect="excel-tab")
				add_authors_to_database(conn, get_sql_author_data(lines))

		if not processed_books:
			with open("raw_open_library/all_metadata.txt") as file:
				lines = csv.reader(file, dialect="excel-tab")
				add_books_to_database(conn, get_sql_book_data(lines))

		create_indices(conn)
	

if __name__ == "__main__":
	create_db()