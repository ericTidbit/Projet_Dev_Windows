classDiagram

    class programme{
        utilisateurs list[utilisateur]
        ouvrirSession()
    }
    class utilisateur{
        nom string
        pwd string
        librairie librairie
        ajouterLivre(Livres)

    }
    class administrateur{

    }
    class client{

    }


    class librairie{
        livres list[livre]
    }
    class livre{
        dateAjout string
        nomlivre string 
        dataLivre string
        Pagelue int
        PagesTotal int
    }
    relation 
    utilisateur <|-- administrateur
    utilisateur <|-- client
    programme *-- utilisateur
    utilisateur *-- librairie
    librairie *-- livre