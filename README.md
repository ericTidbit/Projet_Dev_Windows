# EEEEReader

## Electronic Edition Engine Enhanced Reader



### Import de livres

* Importation fichiers EPUB
* Extraction automatique du titre, de l’auteur et de la couverture (si disponibles)

### Bibliothèque

* Affichage en vignettes ou en liste
* Tri par titre, auteur ou récents
* * Thèmes clair, sombre

### Lecteur (affichage continu)
* Une page par chapitre
* Affichage des images 
* Reprise automatique à la dernière position de lecture


## Diagramme SQL
Note: utilise [Crow's Foot Notation](https://www.freecodecamp.org/news/crows-foot-notation-relationship-symbols-and-how-to-read-diagrams/)

```mermaid

erDiagram
	direction TB
	Utilisateur {
		int Id  ""  
		string Nom  ""  
        	string Pwd "" 
		bool IsAdmin  ""  
		DateTime Date  ""  
	}

	Livre {
		int Id  ""  
		byte[] FichierEpub  ""  
		string Titre  ""  
		string Auteur  ""  
		string Date  ""  
		string ISBN  ""  
		string Langue  ""  
		string Resume  ""  
		byte[] CoverRaw  ""  
        	int CurrentPage ""
        	EpubContent RawContent  "" 
        	Image CoverImage ""
        	List[HtmlDocument] HtmlContentList ""
	}

    	Appli {
        	bool IsDarkMode ""
        	bool IsGridLayout ""
    	}

	Appli ||--o{ Utilisateur : contient
    	Appli ||--|o Utilisateur : contient
   	Appli ||--|o Livre : contient
    	Utilisateur ||--o{ Livre : contient

```
