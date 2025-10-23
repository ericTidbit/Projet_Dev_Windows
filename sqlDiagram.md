erDiagram
    UTILISATEUR ||--o{ SETTINGS : possede
    UTILISATEUR ||--o{ LIVRES : possede
    LIVRES ||--o{ AUTEURS : ref
    LIVRES ||--o{ LANGES : ref

    UTILISATEUR {
        int id
        string name
        string password
        bool isadmin
        DateTime date_creation
    }
    SETTINGS {
        int user_id
        bool dark
        bool grid
    }
    LIVRES {
        int id
        int user_id
        string raw_content
        string Titre
        string auteur_id
        string Date
        string ISBN
        string langue_id
        string Resume
        byte CoverRaw
        int CurrentPage
    }
    AUTEURS {
        int id
        string name
    }
    LANGES{
        int id
        string language
    }
