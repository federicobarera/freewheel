## INTRO

Tech test for FreeWheel

## Dependencies

* .net core 2.1

## How to

The solution has been implemented as per directions.
The only liberty I took regarding the endpoints returning user specific data, is to drive the identity using a bearer token.

For test sake the API accept unsigned tokens.

The following can be used OOB

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJsb2NhbCIsImlzcyI6ImxvY2FsIiwic3ViIjoiMTIzNDU2Nzg5MCIsIm5hbWUiOiJKb2huIERvZSIsInVpZCI6IjEiLCJpYXQiOjE1MTYyMzkwMjJ9.
```

The `uid` parameter can be changed in the token to test different users.