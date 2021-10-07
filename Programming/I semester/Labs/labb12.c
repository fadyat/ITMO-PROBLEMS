#include <stdio.h>

    /*
     *                  HOW TO START WITH COMMAND LINE(Mac OS)
     *
     *  (!) kekw.c - name of my file, test - name of my folder
     *  Open CLion: open -a Clion
     *
     *  You should add to command line path to your code
     *  Like that: cd "/Users/artyom/Clion/test"  <-  path
     *  /Users/artyom/Clion/test is equal to ./test
     *
     *  If you changed your code / not compile yet
     *      you should write this string: clang kekw.c -o test
     *          <- new compilation + add a access to file
     *
     *  Start: ./test argv[0] argv[1] ... argv[n]
     *  argv[0] - path to code
     *  argv[1] ... argv[n] - your arguments
     *
     * */

int main(int argc, char *argv[]) {
    /*
     * At the input we have names of files what we want create
     * Create one more file that list with names of all this files
     */
    FILE *allfiles;
    allfiles = fopen("all.txt", "w");
    for(int i = 1; i < argc; i++) {
        FILE *file;
        file = fopen(argv[i], "w");
        fprintf(file, "%d %s", i, "it's your number");
        fprintf(allfiles, "%s\n", argv[i]);
        fclose(file);
    }
    fclose(allfiles);
}
