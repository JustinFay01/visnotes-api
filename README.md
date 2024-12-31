
# DevOps

## Docker 

### FE 

#### Registry link

### BE

#### Registry link


# TODO

# Front End
- [x] Add framer motion animated list to cloud upload
- [x] Be able to delete files from cloud
- [x] Upload multiple files at once

- [ ] Save options using Zustand
- [ ] Create a mobile friendly Notes Section
- [ ] Add a warning when analyzing a note that has already been analyzed
- [ ] Add a button to generate a new word cloud instead of automatically generating it
- [ ] Save users column widths as a preference
- [ ] Save selected columns as a preference
- [ ] Add a button to download the word cloud as an image
- [ ] Add different Layout options
- [ ] Update the Analysis view when selecting a note

# Back End

## DB Architecture
**Minimum Viable Product Schema**

- [x] Note
    - id
    - name
    - size
    - type
    - url (or path)
    - created_at

- [x] Analysis
    - id
    - note_id
    - created_at
    - raw_value


