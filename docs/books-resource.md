# Books Resource

## Paths

### GET /books

#### Parameters

#### Responses

200 Ok

```json
{
    "data": [
        { 
            "id": 1,
            "title": "Book Title",
            "author": "Joe Schmidit"
        }
    ]
}
```

### GET /books/{id}

#### Parameters

Id: in the url

#### Responses

200 Ok
Formatted as application/json

```json
{
   "id": 1,
   "title":  "Some Title",
   "author": "Some Author",
   "genre": "Fiction"
}

```

404 Not Found


### POST /books

#### Paramaeters

BookPostRequest (In Body)

```json
{
    "title": "Title",
    "author": "Author",
    "genre": "Fiction"
}

```

#### Responses

201 Created

```json
{
   "id": 1,
   "title":  "Some Title",
   "author": "Some Author",
   "genre": "Fiction"
}

```

Headers:

Location: Url of brand new resource
