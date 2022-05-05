grammar Server;

/* fixed */
PACKAGE                     :       'package';
IMPORT                      :       'import';
MODEL_TYPE                  :       'class' | 'interface';
ACCESS_MODIFIER             :       'public' | 'private' | 'protected';
NON_ACCESS_MODIFIER         :       'static' | 'abstract' | 'final';
INHERITANCE_TYPE            :       'extends' | 'implements';

/* ignore */
WS                          :       [ ;\n\t\r] -> skip;

/* main fragments */
fragment LETTER             :       [A-Za-z];  
fragment WORD               :       LETTER+;
fragment DIGIT              :       [0-9];
fragment NUMBER             :       DIGIT+ ('.,' DIGIT+)?;

/* main regex */
NAME                        :       (LETTER | '_') (LETTER | DIGIT | '_')*;
TYPE                        :       (LETTER | '<' | '>' | '.'| DIGIT | '_')+;
PATH                        :       ((WORD '.'?)* '*') | ((WORD '.'?)+ '*'?);

/* key-value */        
KEY_VALUE                   :       NAME ' '? '=' ' '? VALUE;
fragment VALUE              :      ((STRING | TYPE | ANNOTATION_ARGS) ' '?)+;
fragment STRING                      :       '"' ANY+ '"';

/* annotation */
ANNOTATION_HEADER           :       '@' NAME;
fragment ANNOTATION_ARG     :       (KEY_VALUE | VALUE);
ANNOTATION_ARGS             :       '(' (ANNOTATION_ARG ','? ' '?)* ')';

/* any */
ANY                         :       ~([\n;])+?;



/* package & import */
package_name                :       PACKAGE;
import_name                 :       IMPORT;
path                        :       (PATH | NAME | TYPE);
package                     :       package_name path;
import_                     :       import_name path;

/* annotation */
annotation_header           :       ANNOTATION_HEADER;
annotation_arguments        :       ANNOTATION_ARGS;
annotation                  :       annotation_header annotation_arguments?;

/* main func & model */        
modifier                    :       ((ACCESS_MODIFIER NON_ACCESS_MODIFIER?)
                                    | (NON_ACCESS_MODIFIER? ACCESS_MODIFIER));
var_type                    :       (TYPE | NAME);
var                         :       (NAME | KEY_VALUE);
return_type                 :       (TYPE | NAME);

/* model */
model_type                  :       MODEL_TYPE;
model_name                  :       NAME;
parents                     :       INHERITANCE_TYPE (NAME ','?)+;
model_header                :       modifier model_type model_name parents?;
model_attribute             :       annotation* modifier var_type var;
model_annotation            :       annotation;
model                       :       model_annotation* model_header '{' model_attribute* function* '}';
        
/* function */
function_name               :       NAME;
function_arg                :       annotation? var_type var;
function_args               :       '(' (function_arg ','?)* ')' | ANNOTATION_ARGS;
function_header             :       modifier return_type function_name? function_args*;
function_annotation         :       annotation;
function                    :       function_annotation* function_header '{' function_body '}';
function_body               :       ~('}')*;

/* root | start point */
root                        :       (import_ | package | model)* EOF;