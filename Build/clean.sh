echo "Project Cleaner"
echo "Start Cleaning..."

cd ../

rm -Rf *.suo
rm -Rf *.cachefile
rm -Rf *.pidb
rm -Rf *.DS_Store

rm -Rf bin/
rm -Rf obj/

echo "Cleaning done"