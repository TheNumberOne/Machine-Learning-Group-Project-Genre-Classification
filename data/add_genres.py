#!/bin/python

import sqlite3
import pandas as pd
from create_open_library_database import create_connection
import json


def normalize_genre(genre):
    if genre.endswith('.'):
        genre = genre[:-1]
    
    genre = genre.lower()
        
    return genre

def get_genre(conn, title):
    cur = conn.cursor()
    cur.execute("select genres from books where title=? collate nocase", [str(title).lower()])
    rows = cur.fetchall()
    if len(rows) != 1:
        return None
    
    genres = json.loads(rows[0][0])
    genres = { normalize_genre(genre) for genre in genres }
    
    if len(genres) != 1:
        return None
    
    genre, = genres
        
    return genre

def add_genres(data):
    with create_connection() as conn:
        genres = data['title'].apply(lambda x: get_genre(conn, *x), axis=1)
    genres = genres[~genres.isnull()]
    genres_frame = genres.to_frame()
#     print(genres)
    genres_frame.columns = ['genre']
    data_with_genres = pd.merge(data, genres_frame, how='inner', left_index=True, right_index=True)
    
    return data_with_genres
    

data = pd.read_csv("results/gutenberg.csv", index_col="gutenberg id")
add_genres(data).to_csv("results/gutenberg-genres.csv")