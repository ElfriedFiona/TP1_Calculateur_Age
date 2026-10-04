#!/usr/bin/env bash
# À lancer depuis la RACINE du projet MAUI (dossier contenant CalculateurAge.csproj).
# Le dossier "phases" doit être à côté de ce script.
set -e
PH="$(cd "$(dirname "$0")" && pwd)/phases"
REMOTE="https://github.com/ElfriedFiona/TP1_Calculateur_Age.git"

[ -f CalculateurAge.csproj ] || { echo "CalculateurAge.csproj introuvable ici."; exit 1; }

git init -b main 2>/dev/null || true
git remote add origin "$REMOTE" 2>/dev/null || git remote set-url origin "$REMOTE"
if command -v dotnet >/dev/null && [ ! -f .gitignore ]; then dotnet new gitignore >/dev/null; fi

git add -A && git commit -m "Étape 0 : projet .NET MAUI vide (sans sample content)" || true

msgs=(
 "Phase A : version code-behind (MainPage.xaml + OnCalculerClicked)"
 "Phase B : seconde page ResultatPage + navigation Shell (QueryProperty)"
 "Phase C : réécriture en MVVM (BaseViewModel, RelayCommand, CalculateurViewModel)"
 "Phase D : fonctionnalités ajoutées (Majeur/Mineur, Effacer, date future refusée, jours avant anniversaire)"
)
i=0
for P in A B C D; do
  cp -r "$PH/$P/." .
  git add -A
  git commit --allow-empty -m "${msgs[$i]}"
  i=$((i+1))
done

git push -u origin main
