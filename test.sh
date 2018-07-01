DIR=$PWD

cd tests/nunit/
sh test-all.sh $1 $2 $3 $4 $5 &&

cd $DIR
