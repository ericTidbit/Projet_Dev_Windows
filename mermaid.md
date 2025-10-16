classDiagram

    class Programme {
        utilisateurs list[utilisateur]
        ouvrirSession()
    }
    class Utilisateur {
        nom string
        pwd string
        ajouterlibrairie []librairie
    }
    class Administrateur {

    }
    class Client {

    }

    class Librairie {
        livres list[livre]
        ajouterLivre(Livres)
    }
    class Section {
        nom : String
        livres : List[Livre]
        ajouterASection()
    }
    class Livre {
        dateAjout string
        nomlivre string 
        pageLue int
        LivreEntier string
        PagesTotal int
    }

    Utilisateur <|-- Administrateur
    Utilisateur <|-- Client
    Programme *-- Utilisateur
    Utilisateur *-- Librairie
    Librairie *-- Livre
    Librairie *-- Section
    Section --> Livre
