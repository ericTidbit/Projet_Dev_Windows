# Structure MVVM du projet EEEEReader

## Architecture

Ce projet utilise le pattern **MVVM (Model-View-ViewModel)** pour séparer la logique métier de l'interface utilisateur.

## Structure des dossiers

```
EEEEReader/
??? Models/                          # Modèles de données
?   ??? Message.cs                   # Exemple: classe représentant un message
?
??? Views/                           # Vues (interfaces utilisateur XAML)
?   ??? (À créer selon vos besoins)
?
??? ViewModels/                      # ViewModels (logique de présentation)
?   ??? ViewModelBase.cs            # Classe de base pour tous les ViewModels
?   ??? RelayCommand.cs             # Implémentation de ICommand
?   ??? MainViewModel.cs            # ViewModel de la fenêtre principale
?   ?
?   ??? Pages/                       # ViewModels spécifiques aux pages
?       ??? MessagesPageViewModel.cs # ViewModel pour la gestion de messages
?
??? MainWindow.xaml                  # Fenêtre principale
??? MainWindow.xaml.cs              # Code-behind de la fenêtre principale
```

## Composants MVVM

### 1. Models (Modèles)
Les modèles représentent les données de l'application.
- **Message.cs** : Exemple de modèle avec propriétés (Id, Contenu, Auteur, DateCreation)

### 2. ViewModels
Les ViewModels contiennent la logique de présentation et exposent les données aux vues.

#### Classes utilitaires :
- **ViewModelBase** : Classe abstraite de base
  - Implémente `INotifyPropertyChanged`
  - Fournit `OnPropertyChanged()` pour notifier les changements de propriété
  - Fournit `SetProperty()` pour simplifier la mise à jour des propriétés
  
- **RelayCommand** : Implémentation de `ICommand`
  - Permet de lier des actions aux boutons
  - Supporte `CanExecute` pour activer/désactiver les commandes
  - Méthode `LeverChangementPeutExecuter()` pour forcer la réévaluation

#### ViewModels implémentés :
- **MainViewModel** : ViewModel principal de la fenêtre
  - Hérite de `ViewModelBase`
  - Propriétés : 
    - `TexteEntree` : Texte saisi par l'utilisateur
    - `TexteAffiche` : Texte affiché à l'écran
  - Commande : 
    - `CommandeEnvoyer` : Copie le texte d'entrée vers le texte affiché
  
- **MessagesPageViewModel** : ViewModel pour la gestion de messages (exemple)
  - Hérite de `ViewModelBase`
  - Propriétés : 
    - `Messages` : ObservableCollection de messages
    - `MessageSelectionne` : Message actuellement sélectionné
    - `NouveauMessageTexte` : Texte du nouveau message à ajouter
  - Commandes : 
    - `CommandeAjouterMessage` : Ajoute un nouveau message
    - `CommandeSupprimerMessage` : Supprime le message sélectionné

### 3. MainWindow (Fenêtre principale)
La fenêtre principale utilise le MainViewModel :
- **TextBox** lié à `TexteEntree` (bidirectionnel)
- **Button** "Envoyer" lié à `CommandeEnvoyer`
- **TextBlock** lié à `TexteAffiche` (lecture seule)

## Principes MVVM

### 1. Binding (Liaison de données)
Les propriétés du ViewModel sont liées aux contrôles de la vue via `{Binding}` :
```xaml
<!-- Liaison bidirectionnelle pour le TextBox -->
<TextBox Text="{Binding TexteEntree, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>

<!-- Liaison unidirectionnelle pour le TextBlock -->
<TextBlock Text="{Binding TexteAffiche}" />
```

### 2. Commandes
Les actions utilisateur sont gérées par des commandes au lieu d'événements :
```xaml
<Button Command="{Binding CommandeEnvoyer}" Content="Envoyer" />
```

### 3. Notifications de changement
Quand une propriété change, le ViewModel notifie automatiquement la vue via `INotifyPropertyChanged` :
```csharp
public string TexteAffiche
{
    get => _texteAffiche;
    set => SetProperty(ref _texteAffiche, value); // Notifie automatiquement
}
```

La méthode `SetProperty()` de `ViewModelBase` :
- Compare l'ancienne et la nouvelle valeur
- Met à jour le champ si différent
- Appelle `OnPropertyChanged()` pour notifier la vue
- Retourne `true` si la valeur a changé

## Avantages de cette structure

1. **Séparation des préoccupations** : Interface et logique sont séparées
2. **Testabilité** : Les ViewModels peuvent être testés sans interface
3. **Maintenabilité** : Code organisé et facile à modifier
4. **Réutilisabilité** : ViewModelBase et RelayCommand sont réutilisables
5. **Data Binding** : Synchronisation automatique entre vue et données
6. **Pas de code-behind** : Toute la logique est dans les ViewModels

## Guide d'utilisation

### Créer un nouveau ViewModel

```csharp
using System.Windows.Input;

namespace EEEEReader.ViewModels
{
    public class MonViewModel : ViewModelBase
    {
        private string _maPropriete = string.Empty;
        
        public string MaPropriete
        {
            get => _maPropriete;
            set => SetProperty(ref _maPropriete, value);
        }
        
        public ICommand MaCommande { get; }
        
        public MonViewModel()
        {
            MaCommande = new RelayCommand(_ => ExecuterAction());
        }
        
        private void ExecuterAction()
        {
            // Votre logique ici
            MaPropriete = "Nouvelle valeur";
        }
    }
}
```

### Créer une nouvelle Vue (Page WinUI 3)

1. **Créer le fichier XAML** (ex: `MaPage.xaml`) :
```xaml
<Page
    x:Class="EEEEReader.Views.MaPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:vm="using:EEEEReader.ViewModels">

    <Grid>
        <Grid.DataContext>
            <vm:MonViewModel />
        </Grid.DataContext>
        
        <StackPanel>
            <TextBox Text="{Binding MaPropriete, Mode=TwoWay}" />
            <Button Command="{Binding MaCommande}" Content="Action" />
        </StackPanel>
    </Grid>
</Page>
```

2. **Créer le code-behind** (ex: `MaPage.xaml.cs`) :
```csharp
using Microsoft.UI.Xaml.Controls;

namespace EEEEReader.Views
{
    public sealed partial class MaPage : Page
    {
        public MaPage()
        {
            this.InitializeComponent();
        }
    }
}
```

### Utiliser ObservableCollection pour les listes

Pour afficher une liste dynamique qui se met à jour automatiquement :

```csharp
using System.Collections.ObjectModel;

public class MonViewModel : ViewModelBase
{
    public ObservableCollection<Message> Messages { get; }
    
    public MonViewModel()
    {
        Messages = new ObservableCollection<Message>();
    }
    
    private void AjouterMessage()
    {
        Messages.Add(new Message { Contenu = "Nouveau message" });
    }
}
```

```xaml
<ListView ItemsSource="{Binding Messages}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Contenu}" />
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

## Technologies utilisées
- **.NET 8** : Framework de développement
- **WinUI 3** : Interface utilisateur moderne pour Windows
- **C# 12** : Langage de programmation
- **MVVM Pattern** : Architecture de séparation des préoccupations

## Exemple fonctionnel : MainWindow

Le projet contient un exemple fonctionnel dans `MainWindow.xaml` :
- Un `TextBox` pour saisir du texte
- Un `Button` pour envoyer le texte
- Un `TextBlock` pour afficher le texte

L'utilisateur tape du texte, clique sur "Envoyer", et le texte apparaît dans le TextBlock.
Tout est géré via le pattern MVVM sans code dans le code-behind !

## Prochaines étapes

Pour étendre l'application :
1. Créer de nouveaux Models dans `Models/`
2. Créer des ViewModels correspondants dans `ViewModels/` ou `ViewModels/Pages/`
3. Créer des Pages XAML dans `Views/` avec leurs ViewModels
4. Utiliser `Frame.Navigate()` pour naviguer entre les pages si nécessaire
