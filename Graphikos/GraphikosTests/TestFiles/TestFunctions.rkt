;*******************************Line*******************************
(define (line x y x2 y2)
  (if (and (not(= y y2)) (= x x2))
       (if ( < y y2)
          (cons x (cons (+ y 1) (line x (+ y 1) x2 y2)))
          (if ( > y y2)
              (cons x (cons (- y 1) (line x (- y 1) x2 y2)))))
  (if ( = x x2)
      '()
      
      (if (> x x2)
          (cons (+ x2 1) (cons  (+ y2 (/ (- y y2) (- x x2)))
              (line (+ x2 1) (+ y2 (/ (- y y2) (- x x2))) x y )))

          
       (cons (+ x 1) (cons  (+ y (/ (- y2 y) (- x2 x)))
              (line (+ x 1) (+ y (/ (- y2 y) (- x2 x))) x2 y2 )))))))

;Line testfunction
(define test-line
  (lambda (candidate)
    (and (equal? (candidate 0 0 8 8) '(1 1 2 2 3 3 4 4 5 5 6 6 7 7 8 8)))))

;Testrun
(define TestRunLine (test-line line))

;*******************************Rectangle*******************************
(define (rectangle x1 y1 x2 y2)
  (if (= x1 x2)
  '()
  (flatten
  (cons
   (cons
    (cons
     (line x1 y1 x1 y2)
     (line x1 y2 x2 y2))
    (line x2 y2 x2 y1 ))
   (line x1 y1 x2 y1)))
  ))

;flatten
(define (flatten x)
  (cond ((null? x) '())
        ((pair? x) (append (flatten (car x)) (flatten (cdr x))))
        (else (list x))))

;Rectangle testfuntion
(define test-rectangle
  (lambda (candidate)
    (and (equal? (candidate 0 0 2 2) '(0 1 0 2 1 2 2 2 2 1 2 0 1 0 2 0)))))

;TestRun
(define TestRunRectangle (test-rectangle rectangle))

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
    (and (equal? (candidate 5 5 4) '(5 9 5 9 5 1 5 1 9 5 1 5 9 5 1 5 6 9 4 9 6 1 4 1 9 6 1 6 9 4 1 4 7 9 3 9 7 1 3 1 9 7 1 7 9 3 1 3 8 8 2 8 8 2 2 2 8 8 2 8 8 2 2 2)))))

;TestRun
(define TestRunCircle (test-circle circle))