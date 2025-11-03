```
erDiagram
	direction TB
	UTILISATEUR {
		int id  ""  
		string name  ""  
		string password  ""  
		bool isadmin  ""  
		DateTime date_creation  ""  
	}

	SETTINGS {
		int user_id  ""  
		bool dark  ""  
		bool grid  ""  
	}

	LIVRES {
		int id  ""  
		int user_id  ""  
		string raw_content  ""  
		string Titre  ""  
		string auteur_id  ""  
		string Date  ""  
		string ISBN  ""  
		string langue_id  ""  
		string Resume  ""  
		byte CoverRaw  ""  
	}

	AUTEURS {
		int id  ""  
		string name  ""  
	}

	LANGUES {
		int id  ""  
		string language  ""  
	}

	BIBLIOTHEQUE {
		int User_id  ""  
		int Livre_id  ""  
		int CurrentPage  ""  
	}

	UTILISATEUR||--o{SETTINGS:"possede"
	LIVRES||--o{AUTEURS:"ref"
	LIVRES||--o{LANGUES:"ref"
	UTILISATEUR}|--|{BIBLIOTHEQUE:"  "
	BIBLIOTHEQUE}|--|{LIVRES:"  "
```
