#!/bin/bash
curl -d @preferenceChildren.json -X POST http://localhost:5000/preference -H 'Content-Type: application/json'; echo
curl -d @preferenceFamily.json -X POST http://localhost:5000/preference -H 'Content-Type: application/json'; echo
curl -d @preferenceTheater.json -X POST http://localhost:5000/preference -H 'Content-Type: application/json'; echo