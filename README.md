  #WEB_GL_MULTIPLAYER
  
  Acest proiect reprezinta un joc multiplayer in web creat folosind Unity (cu C#) pentru build ul jocului de WEBGL, javascript pentru serverul jucatorilor si python pentru pregatirea fisierelor build ului in web.

  Caracteristici principale:
  - Usor de jucat
  - Disponibil pentru cele mai multe dispozitive
  - Comenzi testate prin testare automata e2e

Scop joc:
Trebuie sa ajungi la linia de finish inaintea altor jucatori.
Primul jucator ce ajunge la linia de finish termina jocul si va aparea pe un text pe ecran daca ai castigat tu sau alt jucator.

#Pasi instalare:
Atentie: Descarca doar folderul Proiect SPS, restul sunt scripturile nebuild uite

1: Deschidere server jucatorii : ruleaza 'node server.js' intr-un command prompt
2: Deschide server fisiere build unity: ruleaza 'py -m http.server 8000' intr-un command prompt si intra
intr-un browser la adresa 'http://localhost:8000/'
3: Have fun


#Structura pe Foldere:
WEB_GL_MULTIPLAYER - folder principal ->

 - Assets -> folderul in care se afla scripturile c# alaturi de obiectele 3D
          -> Scenes -> folderul cu scena jocului
 - Packages -> folder cu pachete necesare comunicarii dintre server si client
 - Proiect SPS -> folderul in care se regaseste serverul (server.js), testul e2e (e2e.js), pagina .html (index.html) si build ul jocului:
               -> Build -> folderul cu fisierele generate de unity continand scena, obiectele si scripturile c#
               -> TemplateData -> contine fisierele necesare paginii web (.css, .png, .ico)
- ProjectSettings -> contine setarile proiectului unity (nume, setari de grafica, build ul selectat <WEBGL> etc)

#Diagrama de arhitectura:

Fisere build <--> Server python http <-> pagina web
      Ʌ
      |
      |
      v
Server javascript players

Serverul pentru playeri creat in javascript va comunica cu fisierele jocului pentru tine cont de numarul de playeri si de pozitia acestora.
Pagina web are legatura doar cu serverul python pentru a primi datele pe care le va afisa pe ecran cat si pentru input ul jucatorului
