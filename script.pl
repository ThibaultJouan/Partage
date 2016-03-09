#!/usr/bin/env perl

use strict;
use warnings;
use threads;
use threads::shared;
#Je pense pas devoir générer 2 tableau pour le mot de passe et l'identifiant, on peut réutiliser le même pour les deux, à voir :)

#Pour ce projet je code les valeurs en dures mais faudrait imaginer un système pour les indiquer via des paramètres
#utiliser une notation comme les expressions régulière : [a..z-A..Z-1..9] si ça existe x)

my @value = ("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l",
"m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x",
"y", "z", 0, 1, 2, 3, 4, 5, 6, 7, 8, 9);

my @tab : shared; #penser un système plus judicieux pour le partage des données.

sub RecursAddValue {
  my $data = shift; #On pourrait tenter de faire moins de boucle for et d'ajouter plus de paramètre à RecursAddValue avec $_[nb]

  if($data < $#value){
    my $thr = threads->new(\&RecursAddValue($data + 1));
    
    for(my $i = 0; $i < @value; $i ++){
      for(my $j = 0; $j < @value; $j ++){
        for(my $k = 0; $k< @value; $k ++){
          my $indice = $value[$data].$value[$i].$value[$j].$value[$k];
          push @tab, $indice; #attention au conflit des différents thread qui bossent avec tab >_< !
        }
      }
    }
  }
}

RecursAddValue(0); #lancement de la fonction récursive :)
foreach my $key (threads->list){ #Nettoyage des threads en cours !
$key->join;
}
print "@tab[0..$#tab]"; #Affichage de toutes les possibilités (./script > fichier.txt)
