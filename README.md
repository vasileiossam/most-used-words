### Filipino 600 most common words
This is an [Anki deck](./Anki/Filipino&#32;most&#32;common&#32;words&#32;based&#32;on&#32;opensubtitles.org.apkg) of about 600 most frequently used words in Filipino. 

Compiled using the tool [here](https://github.com/vasileiossam/most-used-words
) on 170 subtitle files from www.opensubtitles.org. The deck represents 71% of the most used words and it's organized in sub-decks of 50 cards each. 

* About 250 cards and their audio are taken from this [anki deck](https://ankiweb.net/shared/info/40587864).
* https://www.tagalog.com/ was used to get translations from Filipino to English.


### SubtitlesWordCounter

Creates a CSV file with the most frequently used words in subtitle files for non english languages.
It ignores English words using [NetSpell](https://github.com/AiimiLtd/NetSpell).

```
usage: SubtitlesWordCounter <source directory> <output file>

    source directory: directory with .srt files
    output file     : .csv path name
```
#### Examples

* [Top 500 Filipino words](filipino-500-subtitles.md)  
Based on 170 subtitles files from www.opensubtitles.org

### WikipediaWordCounter

Creates a CSV file with the most frequently used words in non English Wikipedia pages.
It ignores English words using [NetSpell](https://github.com/AiimiLtd/NetSpell).

```
usage: WikipediaWordCounter <xml file> <output file>

    xml file    : path name to a wikipedia xml dump file
    output file : .csv path name
```
#### Examples

* [Top 500 Filipino words](filipino-500-wiki.md)  
Based on about 72K pages from the Filipino Wikipedia XML dump (file tlwiki-20210901-pages-meta-current.xml from [tlwiki dump](https://dumps.wikimedia.org/tlwiki/20210901/)).