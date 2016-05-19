;Flatten
(define (flatten x)
  (cond ((null? x) '())
        ((pair? x) (append (flatten (car x)) (flatten (cdr x))))
        (else (list x))))

;Interleave
(define (interleave l1 l2)
      (if (null? l1) l2
          (if (null? l2) l1
              (append (cons (car l1) (cons (cadr l1) '())) (append (cons (car l2) (cons (cadr l2) '())) (interleave (cddr l1) (cddr l2)))))))

;*******************************Line*******************************
(define (line x y x2 y2)
  (let ((xOrg x) (yOrg y) (x2Org x2) (y2Org y2))
  (letrec ((lineCoor (lambda(x y x2 y2 a)
     (if (and (not(= y y2)) (= x x2))
       (if ( < y y2)
          (lineCoor x (+ y 1) x2 y2 (cons x (cons (+ y 1) a )))
          (if ( > y y2)
              (lineCoor x (- y 1) x2 y2 (cons x (cons (- y 1) a)))))
  (if ( = x x2)
      (if (< xOrg x2Org)
       (append (append (cons xOrg '()) (cons yOrg '())) a)
       (cons x2Org (cons y2Org a)))
     (if (> x x2)
             (lineCoor (+ x2 1) (+ y2 (/ (- y y2) (- x x2))) x y
                       (append a (cons (+ x2 1) (cons (round (+ y2 (/ (- y y2) (- x x2)))) '()) )))
              (lineCoor (+ x 1) (+ y (/ (- y2 y) (- x2 x))) x2 y2 
                        (append a (cons (+ x 1)  (cons (round (+ y (/ (- y2 y) (- x2 x)))) '()))))))))))
    (lineCoor x y x2 y2 '() ))))

;Line testfunction
(define test-line
  (lambda (candidate)
    (and (equal? (candidate 0 0 8 8) '(0 0 1 1 2 2 3 3 4 4 5 5 6 6 7 7 8 8))
         (equal? (candidate 8 8 0 0) '(0 0 1 1 2 2 3 3 4 4 5 5 6 6 7 7 8 8))
         (equal? (candidate 0 8 8 0) '(0 8 1 7 2 6 3 5 4 4 5 3 6 2 7 1 8 0))
         (equal? (candidate 8 0 0 8) '(0 8 1 7 2 6 3 5 4 4 5 3 6 2 7 1 8 0))
         (equal? (candidate 1 1 8 1) '(1 1 2 1 3 1 4 1 5 1 6 1 7 1 8 1))
         (equal? (candidate 8 1 1 1) '(1 1 2 1 3 1 4 1 5 1 6 1 7 1 8 1))
         (equal? (candidate 1 8 1 1) '(1 1 1 2 1 3 1 4 1 5 1 6 1 7 1 8))
         (equal? (candidate 1 1 1 8) '(1 8 1 7 1 6 1 5 1 4 1 3 1 2 1 1))
         )))

;Testrun
(define TestRunLine (test-line line))
(for-each display '(Linetest))
TestRunLine
;*******************************Rectangle*******************************
(define (rectangle x1 y1 x2 y2)
  (if (= x1 x2)
  '()
  (append
    (interleave
     (line x1 y1 x1 y2)
     (line x2 y1 x2 y2))
    (interleave
     (line x1 y1 x2 y1 )
   (line x1 y2 x2 y2)))
  ))

;Rectangle testfuntion
(define test-rectangle
  (lambda (candidate)
    (and (equal? (candidate 0 0 3 3) '(0 3 3 3 0 3 3 3 0 2 3 2 0 1 3 1 0 0 0 3 1 0 1 3 2 0 2 3 3 0 3 3))
         (equal? (candidate 0 3 3 0) '(0 0 3 0 0 0 3 0 0 1 3 1 0 2 3 2 0 3 0 0 1 3 1 0 2 3 2 0 3 3 3 0))
         (equal? (candidate 3 0 0 3) '(3 3 0 3 3 3 0 3 3 2 0 2 3 1 0 1 0 0 0 3 1 0 1 3 2 0 2 3 3 0 3 3))
         (equal? (candidate 3 3 0 0) '(3 0 0 0 3 0 0 0 3 1 0 1 3 2 0 2 0 3 0 0 1 3 1 0 2 3 2 0 3 3 3 0))
         )))



;TestRun
(define TestRunRectangle (test-rectangle rectangle))
(for-each display '(Rectangletest))
TestRunRectangle
;*******************************Circle*******************************
(define (circle centerX centerY r)
  (let((x 0) (y r) (d (/ (- 5 (* r 4)) 4)))
      (letrec ((GetCircleCoor (lambda (x y r d)
          (if (<= x y)
               (if (< d 0)    
                  (cons (+ centerX x) (cons (+ centerY y)                                                
                  (cons (- centerX x) (cons (+ centerY y)
                  (cons (+ centerX x) (cons (- centerY y)         
                  (cons (- centerX x) (cons (- centerY y)
                  (cons (+ centerX y) (cons (+ centerY x)
                  (cons (- centerX y) (cons (+ centerY x)
                  (cons (+ centerX y) (cons (- centerY x)
                  (cons (- centerX y) (cons (- centerY x)  
                               (GetCircleCoor (+ x 1) y r (+ d (+ (* x 2) 1)))))))))))))))))))
                  (cons (+ centerX x) (cons (+ centerY y)                                                
                  (cons (- centerX x) (cons (+ centerY y)
                  (cons (+ centerX x) (cons (- centerY y)         
                  (cons (- centerX x) (cons (- centerY y)
                  (cons (+ centerX y) (cons (+ centerY x)
                  (cons (- centerX y) (cons (+ centerY x)
                  (cons (+ centerX y) (cons (- centerY x)
                  (cons (- centerX y) (cons (- centerY x)  
                               (GetCircleCoor (+ x 1) (- y 1) r (+ d (+ (* 2 (- x y)) 1))))))))))))))))))))
               '() ))))
    (GetCircleCoor x y r d))))

;Circle testfunction
(define test-circle
  (lambda (candidate)
    (and (equal? (candidate 5 5 4) '(5 9 5 9 5 1 5 1 9 5 1 5 9 5 1 5 6 9 4 9 6 1 4 1 9 6 1 6 9 4 1 4 7 9 3 9 7 1 3 1 9 7 1 7 9 3 1 3 8 8 2 8 8 2 2 2 8 8 2 8 8 2 2 2))
         (equal? (candidate 15 10 9) '(15 19 15 19 15 1 15 1 24 10 6 10 24 10 6 10 16 19 14 19 16 1 14 1 24 11 6 11 24 9 6 9 17 19 13 19 17 1 13 1 24 12
                                         6 12 24 8 6 8 18 19 12 19 18 1 12 1 24 13 6 13 24 7 6 7 19 18 11 18 19 2 11 2 23 14 7 14 23 6 7 6 20 18 10 18 20 2 10 2 23
                                         15 7 15 23 5 7 5 21 18 9 18 21 2 9 2 23 16 7 16 23 4 7 4 22 17 8 17 22 3 8 3 22 17 8 17 22 3 8 3))
         )))

;TestRun
(define TestRunCircle (test-circle circle))
(for-each display '(Circletest))
TestRunCircle
;*******************************Fill*******************************
(define (fill c g)
    (letrec ((fillCoor (lambda(g x)
        (if (or (or (null? g) (null? (cdr g)) (null? (cddr g))) (null? (cdddr g)))
          (append x (cons c '()))
          (fillCoor (cddddr g)
          (append (line (car g)
                               (cadr g)
                               (car (cddr g))
                               (car (cdddr g))) x))
          ))))
      (fillCoor g '())))

;Fill testfunction
(define test-fill
  (lambda (candidate)
    (and (equal? (candidate "Red" (rectangle 1 1 4 4)) '(4 4 4 4 4 3 4 2 3 4 3 4 3 3 3 2 2 4 2 4 2 3 2 2 1 4 1 4 1 3 1 2 1 2 2 2 3 2 4 2 1 3 2 3 3 3 4 3 1 4 2 4 3 4 4 4 1 4 2 4 3 4 4 4 "Red"))
         (equal? (candidate "Red" (rectangle 4 1 1 4)) '(4 4 4 4 4 3 4 2 3 4 3 4 3 3 3 2 2 4 2 4 2 3 2 2 1 4 1 4 1 3 1 2 1 2 2 2 3 2 4 2 1 3 2 3 3 3 4 3 1 4 2 4 3 4 4 4 1 4 2 4 3 4 4 4 "Red"))
         (equal? (candidate "Red" (rectangle 1 4 4 1)) '(4 1 4 1 4 2 4 3 3 1 3 1 3 2 3 3 2 1 2 1 2 2 2 3 1 1 1 1 1 2 1 3 1 3 2 3 3 3 4 3 1 2 2 2 3 2 4 2 1 1 2 1 3 1 4 1 1 1 2 1 3 1 4 1 "Red"))
         (equal? (candidate "Red" (rectangle 4 4 1 1)) '(4 1 4 1 4 2 4 3 3 1 3 1 3 2 3 3 2 1 2 1 2 2 2 3 1 1 1 1 1 2 1 3 1 3 2 3 3 3 4 3 1 2 2 2 3 2 4 2 1 1 2 1 3 1 4 1 1 1 2 1 3 1 4 1 "Red"))
         (equal? (candidate "Red" (circle 5 5 2)) '(3 4 4 4 5 4 6 4 7 4 3 6 4 6 5 6 6 6 7 6 4 3 5 3 6 3 4 7 5 7 6 7 3 5 4 5 5 5 6 5 7 5 3 5 4 5 5 5 6 5 7 5 5 3 5 7 "Red"))
         )))

;TestRun
(define TestRunFill (test-fill fill))
(for-each display '(Filltest))
TestRunFill
;*******************************Bounding-Box*******************************
(define (removeEverySecondCoordinate l )
      (if (or (null? l) (null? (cdr l)) (null? (cddr l)) (null? (cdddr l)))
          l 
              (flatten (cons (cons (car l) (cadr l)) (removeEverySecondCoordinate (cddddr l))))))


(define (bounding-box x1 y1 x2 y2)
  (if (= x1 x2)
  '() 
   (append
    (interleave
     (removeEverySecondCoordinate (line x1 y1 x1 y2))
     (removeEverySecondCoordinate (line x2 y1 x2 y2)))
    (interleave
     (removeEverySecondCoordinate (line x1 y1 x2 y1 ))
     (removeEverySecondCoordinate (line x1 y2 x2 y2)))
  )))

;filter
(define (filter x1 y1 x2 y2 l)
  (letrec ((filterCoor (lambda (x1 y1 x2 y2 l a)
  (if (or (null? l) (not (real? (car l)))) 
      (flatten (append a l)) 
      (if (and (and (and (>= (car l) x1) (<= (car l) x2) (>= (cadr l) y1))) (<= (cadr l) y2))
           (filterCoor x1 y1 x2 y2 (cddr l) (cons (cons (car l) (cadr l)) a))
           (filterCoor x1 y1 x2 y2 (cddr l) a )))))) 
    (filterCoor x1 y1 x2 y2 l '())))

;Testfunction
(define test-boundingbox
  (lambda (candidate)
    (and (equal? (candidate 1 1 8 8) '(1 8 8 8 1 7 8 7 1 5 8 5 1 3 8 3 1 1 1 8 3 1 3 8 5 1 5 8 7 1 7 8))
         (equal? (candidate 1 8 8 1) '(1 1 8 1 1 2 8 2 1 4 8 4 1 6 8 6 1 8 1 1 3 8 3 1 5 8 5 1 7 8 7 1))
         (equal? (candidate 8 1 1 8) '(8 8 1 8 8 7 1 7 8 5 1 5 8 3 1 3 1 1 1 8 3 1 3 8 5 1 5 8 7 1 7 8))
         (equal? (candidate 8 8 1 1) '(8 1 1 1 8 2 1 2 8 4 1 4 8 6 1 6 1 8 1 1 3 8 3 1 5 8 5 1 7 8 7 1))
         )))

;TestRun
(define TestRunBB (test-boundingbox bounding-box))
(for-each display '(Boundingboxtest))
TestRunBB
;*******************************Text*******************************
(define (text-at x1 y1 t)
  (cons  x1 (cons y1 (cons t '()))))

;testfunction
(define test-text
  (lambda (candidate)
    (and (equal? (candidate 7 3 "Så er der gang i den") '(7 3 "Så er der gang i den")))))

;TestRun
(define TestRunText (test-text text-at))
(for-each display '(Texttest))
TestRunText
;*******************************Draw*******************************
(define (draw . g)
  (flatten g))

;testfunction
(define test-draw
  (lambda (candidate)
   (and (equal? (candidate "Red" (line 1 1 4 4) (rectangle 1 5 5 1)) '("Red" 1 1 2 2 3 3 4 4 1 1 5 1 1 1 5 1 1 2 5 2 1 3 5 3 1 4 5 4 1 5 1 1 2 5 2 1 3 5 3 1 4 5 4 1 5 5 5 1)))))

;TestRun
(define TestRunDraw (test-draw draw))
(for-each display '(Drawtest))
TestRunDraw