# most-used-words

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