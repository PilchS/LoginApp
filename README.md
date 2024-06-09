# LoginApp

LoginApp is project which goal was to prevent user to do an SQL injection. 
---
I managed to do that by hashing the username and password. When user wrote their credentials, the program hashes prvided data, and then checks it with ones in the database.
For it to work for you you need a database that looks like this:

| id |   username   |       password       |                   username_hash                    |                   password_hash                    |   salt   |
| -- | ------------ | -------------------- | -------------------------------------------------- | -------------------------------------------------- | -------- |             
|  1 | test         | test                 | #2lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFgHiJlMf2 | $efsaMlMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCkePQ7u | nFx5Cc7P |
|  2 | Ben123       | Passwd1.             | #!alMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFgHiJ82& | $XfFjD1YlMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXy8lZJSmHI | 8kMTFnUf |
|  3 | Carl         | IfailedTheTask       | #3lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFgHiJlMe@ | $OnYynqvuDGlMoPqRsTvWxYzaCdEfGhJkLmNoQrcfRQz1QtbWO | DR13gOJi |
|  4 | MarthaG      | Pupp1es              | #&elMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFgHiJ2n! | $ycs1NflMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZFKwFFedj | eXMSyrgw |
|  5 | DarkWarior66 | Bl0Od                | #0e#J9lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCe*b&68 | $Q0xqPlMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbUtrcco3 | YHweYp13 |
|  6 | Jeff         | 2Mv5kf9RZ2Kfro3sSgjL | #&lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFgHiJlMs! | $Xifxa9mMjXCeXlMoPqRsTvWxYzaCdEfGhJk3yFCwF1ZHLMirm | 19UTZyeR |
|  7 | Student      | Passed               | #$hlMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFgHi*r2g | $hfbrz3lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAvBStjoH | 3mOug6bm |
|  8 | JS           | React                | #lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFgHiJlMnO# | $LnjgblMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbZxZmtbI | KMsMEgpV |
| 9  | IJustLearned | C#                   | #9h!g5lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCr&e^r8 | $pJf7lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFZpBIq | Ww7mcEV3 |
| 10 | All          | users                | #lMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbCeFgHiJlMn9y | $irVfglMoPqRsTvWxYzaCdEfGhJkLmNoQrStUvXyZAbaJTOTB5 | wndGiGz5 |

(for getting correct hashed data and salt, go to my other program "HashingProgram": https://github.com/PilchS/HashingProgram )

 When you have a database, provide your credentials in line 11 in AccountController.cs file
