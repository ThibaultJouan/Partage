#!/usr/bin/env perl

use strict;
use warnings;
use threads;
use threads::shared;

 sub sub1{
   print "Nous voila dans le thread numéro " .$_[0] . "! \n";
 }

my $thr = threads->new(\&sub1(0));
my $thr1 = threads->new(\&sub1(1));
my $thr2 = threads->new(\&sub1(2));

#l'execution du programme affiche des warnings non attendu :O
